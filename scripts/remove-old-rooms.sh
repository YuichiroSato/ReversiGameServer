#!/bin/bash

TARGET_DIR="/tmp/reversi"
YESTERDAY=$(date -d "yesterday" +%s)
files=$(ls -1p $TARGET_DIR | grep -v "/")

for file in $files; do
  last_modified=$(date -r $TARGET_DIR/$file +%s)
  if [ $last_modified -lt $YESTERDAY ]; then
    rm $TARGET_DIR/$file
    rm $TARGET_DIR/state/$file
    echo "File $file was deleted"
   fi
done
