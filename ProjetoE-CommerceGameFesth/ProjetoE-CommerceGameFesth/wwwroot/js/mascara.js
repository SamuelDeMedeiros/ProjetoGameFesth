// responsavel por exibir a mascara
$(document).ready(function () {

    $('#dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    $('#data').mask("00/00/0000", { placeholder: "__/__/____" });

    $('#dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    $('#cpf').mask('000.000.000-00', { reverse: true });

    $('#rg').mask('00.000.000-0');

    $('#cep').mask('00000-000');

    $('#telefone').mask('(00) 00000-0000');

    $('#cnpj').mask('00.000.000/0000-00');

});