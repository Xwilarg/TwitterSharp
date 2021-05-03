#!/bin/sh
set -e
cd TwitterSharp.UnitTests/TestResults
cd $(ls)
cp coverage.cobertura.xml  ../../../coverage.xml
cd ../../..