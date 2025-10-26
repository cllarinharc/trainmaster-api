*** Settings ***
Library    SeleniumLibrary

*** Variables ***
${URL_LOGIN}          http://127.0.0.1:5215/Login
${URL_CURSOS}         http://127.0.0.1:5215/Cursos
${BROWSER}            chrome

${CPF}                04052972384
${SENHA}              qwer12

${INPUT_CPF}          //*[@id='Cpf']
${INPUT_SENHA}        //*[@id='Password']
${BOTAO_ENTRAR}       //button[contains(text(),'Entrar')]

${BOTAO_NOVO_CURSO}   //button[contains(text(),'+ Novo Curso')]

${INPUT_NOME}         //*[@name='Name']
${INPUT_DESCRICAO}    //*[@name='Description']
${INPUT_START_DATE}   //*[@name='StartDate']
${INPUT_END_DATE}     //*[@name='EndDate']
${BTN_SALVAR}         //button[contains(text(),'Salvar')]

*** Keywords ***
Abrir o navegador e fazer login
    Open Browser    ${URL_LOGIN}    ${BROWSER}
    Maximize Browser Window
    Sleep           2s
    Input Text      ${INPUT_CPF}    ${CPF}
    Input Text      ${INPUT_SENHA}  ${SENHA}
    Click Button    ${BOTAO_ENTRAR}
    Sleep           2s
  Capture Page Screenshot

Navegar até página de cursos
    Click Link      Cursos
    Sleep           2s
  Capture Page Screenshot

Acessar tela de cadastro de curso
    Wait Until Element Is Visible    ${BOTAO_NOVO_CURSO}    10s
    Click Element                    ${BOTAO_NOVO_CURSO}
    Sleep                            2s
  Capture Page Screenshot

Preencher dados do curso
    Input Text      ${INPUT_NOME}        11111111111111111111111111111111111111111111
    Input Text      ${INPUT_DESCRICAO}   Automação de testes com Robot Framework Avançado
    Input Text      ${INPUT_START_DATE}  22/05/2025
    Input Text      ${INPUT_END_DATE}    31/05/2025    
    Sleep           1s
  Capture Page Screenshot

Salvar cadastro
    Click Element   ${BTN_SALVAR}
    Sleep           2s
    Capture Page Screenshot

Fechar navegador
    Close Browser

*** Test Cases ***
Cenário: Adicionar um novo curso com sucesso
    [Tags]    cadastro    curso    positivo
    Abrir o navegador e fazer login
    Navegar até página de cursos
    Acessar tela de cadastro de curso
    Preencher dados do curso
    Salvar cadastro
    Fechar navegador
