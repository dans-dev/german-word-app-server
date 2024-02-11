#!/bin/bash

read -p "Enter MySQL server IP: " serverIP
read -p "Enter MySQL admin username: " adminUsername
read -s -p "Enter MySQL admin password: " adminPassword
echo
read -p "Enter database username: " dbUsername
read -s -p "Enter database password: " dbPassword
echo
read -p "Enter database name: " dbName

configFile="database_config.txt"
echo "Server IP: $serverIP" > "$configFile"
echo "MySQL Admin Username: $adminUsername" >> "$configFile"
echo "MySQL Admin Password: $adminPassword" >> "$configFile"
echo "Database Username: $dbUsername" >> "$configFile"
echo "Database Password: $dbPassword" >> "$configFile"
echo "Database Name: $dbName" >> "$configFile"

mysql -h "$serverIP" -u "$adminUsername" -p"$adminPassword" -e "CREATE DATABASE IF NOT EXISTS \`$dbName\`;"
mysql -h "$serverIP" -u "$adminUsername" -p"$adminPassword" -e "CREATE USER '$dbUsername'@'%' IDENTIFIED BY '$dbPassword';"
mysql -h "$serverIP" -u "$adminUsername" -p"$adminPassword" -e "GRANT ALL PRIVILEGES ON \`$dbName\`.* TO '$dbUsername'@'%';"

mysql -h "$serverIP" -u "$dbUsername" -p"$dbPassword" "$dbName" << EOF
CREATE TABLE IF NOT EXISTS WordSets (
    WordSetID INT AUTO_INCREMENT PRIMARY KEY,
    WordSetName VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS Words (
    WordID INT AUTO_INCREMENT PRIMARY KEY,
    WordSetID INT,
    EnglishWord VARCHAR(255),
    GermanWord VARCHAR(255),
    WordType VARCHAR(50),
    FOREIGN KEY (WordSetID) REFERENCES WordSets(WordSetID)
);
EOF

echo "Database configuration saved to $configFile"
echo "Database, user, and tables created successfully."
