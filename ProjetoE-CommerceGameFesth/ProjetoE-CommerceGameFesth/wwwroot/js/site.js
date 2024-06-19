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

    $('#ie').mask('000.000.000.000');

});
$(document).ready(function () {

    function limpa_formulário_cep() {
        // Limpa valores do formulário de cep.
        $("#UF").val("");
        $("#Cidade").val("");
        $("#Lougradouro").val("");
        $("#Bairro").val("");
    }

    //Quando o campo cep perde o foco.
    $("#cep").blur(function () {

        //Nova variável "cep" somente com dígitos.
        var cep = $(this).val().replace(/\D/g, '');

        //Verifica se campo cep possui valor informado.
        if (cep != "") {

            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;

            //Valida o formato do CEP.
            if (validacep.test(cep)) {

                //Preenche os campos com "..." enquanto consulta webservice.
                $("#UF").val("...");
                $("#Cidade").val("...");
                $("#Lougradouro").val("...");
                $("#Bairro").val("...");

                //Consulta o webservice viacep.com.br/
                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#UF").val(dados.uf);
                        $("#Cidade").val(dados.localidade);
                        $("#Lougradouro").val(dados.logradouro);
                        $("#Bairro").val(dados.bairro);
                    } //end if.
                    else {
                        //CEP pesquisado não foi encontrado.
                        limpa_formulário_cep();
                        alert("CEP não encontrado.");
                    }
                });
            } //end if.
            else {
                //cep é inválido.
                limpa_formulário_cep();
                alert("Formato de CEP inválido.");
            }
        } //end if.
        else {
            //cep sem valor, limpa formulário.
            limpa_formulário_cep();
        }
    });
});