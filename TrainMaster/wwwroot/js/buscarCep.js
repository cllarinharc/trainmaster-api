document.addEventListener('DOMContentLoaded', function () {
    const buscarCepButton = document.getElementById('buscarCep');

    if (buscarCepButton) {
        buscarCepButton.addEventListener('click', function () {
            const cepInput = document.getElementById('PostalCode');
            let cep = cepInput.value.replace(/\D/g, '');

            if (cep === '') {
                alert('Digite um CEP.');
                return;
            }

            fetch(`/Endereco/GetByPostalCode?postalCode=${cep}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('CEP não encontrado.');
                    }
                    return response.json();
                })
                .then(data => {
                    document.querySelector('input[name="Street"]').value = data.street || '';
                    document.querySelector('input[name="Neighborhood"]').value = data.neighborhood || '';
                    document.querySelector('input[name="City"]').value = data.city || '';
                    document.querySelector('input[name="Uf"]').value = data.uf || '';
                })
                .catch(error => {
                    alert(error.message);
                });
        });
    }
});
