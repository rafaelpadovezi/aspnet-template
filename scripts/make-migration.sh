#!/usr/bin/env bash
set -e

PROJECT=src/AspnetTemplate.Core
STARTUP_PROJECT=src/AspnetTemplate.Api

echo "Creating migration $1..."

dotnet ef migrations add $1 \
  --project ${PROJECT} \
  --startup-project ${STARTUP_PROJECT} \
  --output-dir Database/Migrations