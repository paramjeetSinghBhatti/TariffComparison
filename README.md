# TariffComparison Web API

This is a Web API built with .NET 8 that allows users to compare various tariff plans based on consumption. The API provides endpoint to calculate different Tariifs Annual Costs using different calculators and serves results in JSON format.

## Table of Contents

- [Project Description](#project-description)
- [Installation](#installation)
- [Usage](#usage)

## Project Description

The **TariffComparison** project is a RESTful Web API designed to compare various tariff plans, including basic, packaged, and other custom tariff calculators. Built with .NET 8, it provides a flexible and extendable solution to calculate tariffs based on different input parameters.

This API supports different tariff calculators:
- `BasicTariffCalculator`
- `PackagedTariffCalculator`
- `CustomTariffCalculator` (commented out in the configuration)

This API supports asynchronous operations for optimized performance in high-traffic scenarios.

## Installation
### Windows

Prerequisites
Before you begin, ensure you have the following installed:
- .NET 8 SDK
- Git

Steps to Install
1. **Clone the repository: Public Repo** 
    https://github.com/paramjeetSinghBhatti/TariffComparison.git
2. Run the below commands in the Solution root directory
    - dotnet build -c Release
    - dotnet publish ./TariffComparison/TariffComparison.csproj -c Release -o out
    - cd out
    - $env:ASPNETCORE_ENVIRONMENT="Development" `You can optionally set it to Production as well.`
    - dotnet TariffComparison.dll --urls "http://localhost:5226;https://localhost:7263"

### Linux

Prerequisites
- Script: [https://github.com/paramjeetSinghBhatti/TariffComparison/blob/5fc8a935040f159da1213a72845603a1d1287dec/TariffComparison/Script/setup_webApi.sh](https://github.com/paramjeetSinghBhatti/TariffComparison/blob/0a21e8670c34b83c9a14ba64487c85fc68075caf/setup_WebApi.sh)

- Git Installed
- Ubuntu or Debian-Based System `I have tested the script on Ubuntu's Latest version`

- Steps to run API
    1. Switch to the directory containing the script.
    2. Run the script with the command: sh <script-name> <Git repo URL> `eg. sh setup_webApi.sh https://github.com/paramjeetSinghBhatti/TariffComparison`

## Usage
    `API endpoint: /api/tariffcomparison/compare?consumption=4000`
