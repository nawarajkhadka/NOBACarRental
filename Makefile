SOLUTION := CarRental.slnx
API_PROJECT := src/CarRental.Api/CarRental.Api.csproj
TEST_PROJECT := tests/CarRental.Tests/CarRental.Tests.csproj

.PHONY: help build test run clean restore docker-up docker-down docker-logs

help:
	@echo "make build        - restore and build the solution"
	@echo "make test         - run the test suite"
	@echo "make run          - run the API locally (dotnet run)"
	@echo "make clean        - remove build artifacts"
	@echo "make docker-up    - build and start the API + SQL Server via docker compose"
	@echo "make docker-down  - stop and remove docker compose containers"
	@echo "make docker-logs  - follow docker compose logs"

restore:
	dotnet restore $(SOLUTION)

build: restore
	dotnet build $(SOLUTION) --no-restore

test:
	dotnet test $(TEST_PROJECT)

run:
	dotnet run --project $(API_PROJECT)

clean:
	dotnet clean $(SOLUTION)

docker-up:
	docker compose up --build

docker-down:
	docker compose down

docker-logs:
	docker compose logs -f
