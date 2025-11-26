const cpfInput = document.getElementById('cpf');
//impede letras no CPF, limita para 11 números e já aplica a máscara 000.000.000-00 enquanto o usuário digita.
if (cpfInput) {
    cpfInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 11) value = value.slice(0, 11);

        value = value
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d{1,2})$/, '$1-$2');

        e.target.value = value;
    });
}
