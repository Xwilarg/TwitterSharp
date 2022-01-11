#!/bin/sh
set -e
cd test/TestResults
cd $(ls)
cp coverage.cobertura.xml  ../../../coverage.xml
cd ../../..