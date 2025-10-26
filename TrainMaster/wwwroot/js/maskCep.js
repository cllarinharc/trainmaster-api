document.addEventListener('DOMContentLoaded', function () {
    const cepInput = document.getElementById('PostalCode');
    const buscarCepButton = document.getElementById('buscarCep');

    // Máscara para CEP enquanto digita
    cepInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 8) {
            value = value.slice(0, 8);
        }

        // Aplica a máscara no formato 00000-000
        if (value.length > 5) {
            value = value.replace(/(\d{5})(\d)/, '$1-$2');
        }

        e.target.value = value;
    });

    // Função buscar CEP
    if (buscarCepButton) {
        buscarCepButton.addEventListener('click', function () {
            const cep = cepInput.value.replace(/\D/g, '');

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