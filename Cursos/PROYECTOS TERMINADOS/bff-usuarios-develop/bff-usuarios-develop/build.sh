dotnet restore "Spv.Usuarios.Bff.Api/Spv.Usuarios.Bff.Api.csproj"
dotnet build "Spv.Usuarios.Bff.Api/Spv.Usuarios.Bff.Api.csproj" -c Release

dotnet test Spv.Usuarios.Bff.Test.Unit/Spv.Usuarios.Bff.Test.Unit.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover --test-adapter-path:. --logger:"junit;LogFilePath=..\test_results\spv-test-unit-test-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" /p:CoverletOutput=../test_results/Usuarios.Bff.Test.Unit.opencover.xml
dotnet test Spv.Usuarios.Bff.Test.Integration/Spv.Usuarios.Bff.Test.Integration.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover --test-adapter-path:. --logger:"junit;LogFilePath=..\test_results\spv-test-unit-integration-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" /p:CoverletOutput=../test_results/Usuarios.Bff.Test.Integration.opencover.xml
