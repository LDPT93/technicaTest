# technicaTest
Build docker images at the same level path to `src`:


sh

docker build -t producer -f ./src/EncrypterDateProducer/Dockerfile .
docker build -t consumer -f ./src/DecrypterDateConsumer/Dockerfile .
docker run -e "DOTNET_ENVIRONMENT=Development"  producer