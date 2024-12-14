#!/bin/bash

# Script to set up .NET 8 environment and run a Web API on a Linux machine
# This script assumes Ubuntu/Debian-based systems. Modify for other distributions if necessary.

set -e  # Exit on error

# 1. Update package list and upgrade system
echo "Updating package list and upgrading system..."
sudo apt update && sudo apt upgrade -y

# 2. Install required packages
echo "Installing required packages..."
sudo apt install -y wget curl apt-transport-https software-properties-common

# 3. Install .NET 8 SDK and runtime
echo "Adding Microsoft package repository and installing .NET 8..."
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Verify installation
dotnet --version

# 4. Install other dependencies (e.g., for database support, build tools)
echo "Installing additional dependencies..."
sudo apt install -y build-essential libssl-dev libcurl4-openssl-dev zlib1g-dev

# 5. Clone and build the Web API project
echo "Cloning the Web API project repository..."
if [ -z "$1" ]; then
    echo "Usage: $0 https://github.com/paramjeetSinghBhatti/TariffComparison"
    exit 1
fi

# Clone the repository
git clone "$1" webapi-project
cd webapi-project

# Restore dependencies using the solution file
echo "Restoring dependencies..."
dotnet restore EnergyTariffComparison.sln

# Build the solution in Release configuration
echo "Building the solution..."
dotnet build EnergyTariffComparison.sln -c Release

# Publish the project(s) in the solution to the 'out' folder
echo "Publishing the project..."
dotnet publish EnergyTariffComparison.sln -c Release -o out

# 6. Run the Web API
echo "Running the Web API..."
cd out
dotnet TariffComparison.dll  # Replace <your-webapi-dll> with your Web API's DLL name.

# Done
echo "Setup complete. Your .NET 8 Web API is running."
