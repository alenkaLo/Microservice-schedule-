#!/bin/bash
max_backups=200
if [[ "$(ls /root/backups/ | wc -l)" -ge "$max_backups" ]]; then
    count_to_delete=$(($(ls /root/backups/ | wc -l)-"$max_backups"+1));
    find /root/backups -mindepth 1 | head -"$count_to_delete" | xargs -r rm;
fi
docker exec microservice-schedule--db-1 pg_dumpall -U "postgres" > /root/backups/"$(date +%s)___$(date +%F-%T-%Z)"