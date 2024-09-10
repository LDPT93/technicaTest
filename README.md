# technicaTest
Build docker images at the same level path to `src`:


sh

docker compose up
docker compose down

docker build -t producer -f ./src/EncrypterDateProducer/Dockerfile .
docker run --network=host -e "DOTNET_ENVIRONMENT=Prod"  producer

docker build -t consumer -f ./src/DecrypterDateConsumer/Dockerfile .
docker run --network=host -e "DOTNET_ENVIRONMENT=Prod"  consumer
