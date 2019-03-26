## Utilizando

Copie o arquivo **Intranet.Template.1.0.0** que se encontra no servidor de projetom em: `\\projetos\PROJETOS\Desenvolvimento\Templates` e adicione em:

 `C:\Users\Usuario\Documents\Visual Studio 2017\Templates\ProjectTemplates`

 Lembrando que o diretório pode variar de acordo com o usuário e versão do VSTS.

 Depois que o zip estiver na pasta abra o Visual Studio e crie um projeto do tipo `Intranet.Template.1.0.0`

 Após a criação do projeto edite o `.csproj` do projeto e altere:

 De:
 ```
     <Reference Include="System.Web.Mvc">
      <HintPath>..\..\..\..\..\..\..\Program Files (x86)\Microsoft ASP.NET\ASP.NET MVC 4\Assemblies\System.Web.Mvc.dll</HintPath>
    </Reference>
 ```

 Para:
  ```
    <Reference Include="System.Web.Mvc">
      <HintPath>$(SolutionDir)packages\Microsoft.AspNet.Mvc.5.2.3\lib\net45\System.Web.Mvc.dll</HintPath>
    </Reference>
 ```


