#!/bin/sh
printf '\033c\033]0;%s\a' dedicated server Photon Fusion sdk 3.0.0
base_path="$(dirname "$(realpath "$0")")"
"$base_path/dedicated server Photon Fusion sdk 3.0.0.x86_64" "$@"
