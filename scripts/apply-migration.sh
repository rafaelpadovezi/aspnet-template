#!/usr/bin/env bash
set -e

PROJECT=src/AspnetTemplate.Core
STARTUP_PROJECT=src/AspnetTemplate.Api
[[ $@ =~ "-v" || $@ =~ "--verbose" ]] && VERBOSE_PARAM="--verbose" || VERBOSE_PARAM=""

echo "Applying migrations..."
dotnet ef database update --project ${PROJECT} --startup-project ${STARTUP_PROJECT} $VERBOSE_PARAM