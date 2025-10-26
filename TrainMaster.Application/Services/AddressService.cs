using Newtonsoft.Json;
using RestSharp;
using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Shared.Logging;
using TrainMaster.Shared.Validator;

namespace TrainMaster.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public AddressService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<AddressEntity>> Add(AddressEntity addressEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var isValidAddress = await IsValidAddressRequest(addressEntity);

                if (!isValidAddress.Success)
                {
                    Log.Error(LogMessages.InvalidAddressInputs());
                    return Result<AddressEntity>.Error(isValidAddress.Message);
                }

                if (!IsValidCep(addressEntity.PostalCode))
                {
                    Log.Error("Postal code is invalid.");
                    return Result<AddressEntity>.Error("Postal Code invalid.");
                }

                addressEntity.ModificationDate = DateTime.UtcNow;
                var result = await _repositoryUoW.AddressRepository.Add(addressEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();               

                return Result<AddressEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.AddingAddressError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to add a new address");
            }
            finally
            {
                Log.Error(LogMessages.AddingAddressSuccess());
                transaction.Dispose();
            }
        }

        public async Task Delete(int userId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var addressEntity = await _repositoryUoW.AddressRepository.GetById(userId);
                if (addressEntity is not null)
                    _repositoryUoW.AddressRepository.Update(addressEntity);

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.DeleteAddressError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to delete a Address.");
            }
            finally
            {
                Log.Error(LogMessages.DeleteAddressSuccess());
                transaction.Dispose();
            }
        }

        public async Task<List<AddressEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                List<AddressEntity> addressEntities = await _repositoryUoW.AddressRepository.Get();
                _repositoryUoW.Commit();
                return addressEntities;
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.GetAllAddressError(ex));
                transaction.Rollback();
                throw new InvalidOperationException("Error to loading the list Address");
            }
            finally
            {
                Log.Error(LogMessages.GetAllAddressSuccess());
                transaction.Dispose();
            }
        }

        public async Task<Result<AddressEntity>> GetById(int id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var result = await _repositoryUoW.AddressRepository.GetById(id)
                             ?? new AddressEntity();

                _repositoryUoW.Commit();
                return Result<AddressEntity>.Okedit(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao buscar endereço por ID", ex);
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public async Task<Result<AddressEntity>> Update(int id,AddressEntity addressEntity)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var addressById = await _repositoryUoW.AddressRepository.GetById(id);

                if (addressById is null)
                {
                    addressEntity.CreateDate = DateTime.UtcNow;
                    addressEntity.ModificationDate = DateTime.UtcNow;
                    addressEntity.PessoalProfileId = id;
                    await _repositoryUoW.AddressRepository.Add(addressEntity);
                }
                else
                {
                    addressById.PostalCode = addressEntity.PostalCode;
                    addressById.Street = addressEntity.Street;
                    addressById.City = addressEntity.City;
                    addressById.Uf = addressEntity.Uf;
                    addressById.Neighborhood = addressEntity.Neighborhood;
                    addressById.ModificationDate = DateTime.UtcNow;

                    _repositoryUoW.AddressRepository.Update(addressById);
                }

                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                return Result<AddressEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(LogMessages.UpdatingErrorAddress(ex));
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error creating or updating Address", ex);
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }


        public async Task<Result<AddressEntity>> GetAddressByZipCode(string postalCode)
        {
            try
            {
                if (!IsValidCep(postalCode))
                {
                    Log.Error("Postal code is invalid.");
                    return Result<AddressEntity>.Error("Postal Code invalid.");
                }

                string url = $"https://viacep.com.br/ws/{postalCode}/json/";

                var client = new RestClient();
                var request = new RestRequest(url, RestSharp.Method.Get);

                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
                {
                    var endereco = JsonConvert.DeserializeObject<AddressEntity>(response.Content);

                    if (endereco == null || string.IsNullOrWhiteSpace(endereco.PostalCode))
                        return Result<AddressEntity>.Error("Postal code not found.");

                    return Result<AddressEntity>.Ok("Postal code found.", endereco);
                }

                return Result<AddressEntity>.Error("Error fetching postal code.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error fetching postal code: {ex.Message}");
                return Result<AddressEntity>.Error("Error occurred while fetching postal code.");
            }
        }

        private async Task<Result<AddressEntity>> IsValidAddressRequest(AddressEntity addressEntity)
        {
            var requestValidator = await new AddressRequestValidator().ValidateAsync(addressEntity);
            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<AddressEntity>.Error(errorMessage);
            }

            return Result<AddressEntity>.Ok();
        }

        private bool IsValidCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;
            cep = cep.Trim();
            return cep.Length == 8 && cep.All(char.IsDigit);
        }
    }
}