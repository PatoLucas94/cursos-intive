dotnet restore "Spv.Usuarios.Api/Spv.Usuarios.Api.csproj"
dotnet build "Spv.Usuarios.Api/Spv.Usuarios.Api.csproj" -c Release

dotnet test Spv.Usuarios.Test.Unit/Spv.Usuarios.Test.Unit.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover --test-adapter-path:. --logger:"junit;LogFilePath=..\test_results\spv-test-unit-test-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" /p:CoverletOutput=../test_results/Usuarios.Test.Unit.opencover.xml
dotnet test Spv.Usuarios.Test.Integration/Spv.Usuarios.Test.Integration.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover --test-adapter-path:. --logger:"junit;LogFilePath=..\test_results\spv-test-unit-integration-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" /p:CoverletOutput=../test_results/Usuarios.Test.Integration.opencover.xml