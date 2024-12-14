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

### Features:
- Easily extendable with new tariff calculators.
- Swagger UI for interactive API documentation.
- Supports both HTTP and HTTPS endpoints.

- ## Installation - Windows

### Prerequisites
Before you begin, ensure you have the following installed:
- .NET 8 SDK
- Git

### Steps to Install
1. **Clone the repository: Public Repo** 
https://github.com/paramjeetSinghBhatti/TariffComparison.git
