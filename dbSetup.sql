CREATE TABLE
    IF NOT EXISTS accounts(
        id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
        createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
        updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
        name varchar(255) COMMENT 'User Name',
        email varchar(255) COMMENT 'User Email',
        picture varchar(255) COMMENT 'User Picture'
    ) default charset utf8 COMMENT '';

ALTER TABLE accounts
ADD
    COLUMN coverImg VARCHAR(255) NOT NULL DEFAULT "https://media.istockphoto.com/id/1141304089/vector/global-network-connection-concept-big-data-visualization-social-network-communication-in-the.jpg?s=612x612&w=0&k=20&c=XoQRixTQExBgu3CKeSzTleQSbasbpBkDs3S3q6BByOM=";

CREATE TABLE
    IF NOT EXISTS keeps(
        id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
        creatorId VARCHAR(260) NOT NULL,
        name VARCHAR(50) NOT NULL,
        description VARCHAR(300) NOT NULL,
        img VARCHAR(255) NOT NULL DEFAULT "https://images.ctfassets.net/b4k16c7lw5ut/TdKLskzZYA5IJddHkSvQE/23850e128cb6c060851b2371ccfabef3/Clipchamp-Pinterest-partnership-step-3.jpg?w=1000&h=563&fl=progressive&q=50&fm=jpg",
        views INT NOT NULL DEFAULT 0,
        kept INT NOT NUll DEFAULT 0,
        FOREIGN KEY(creatorId) REFERENCES accounts(id) ON DELETE CASCADE
    ) default charset utf8 COMMENT '';

CREATE TABLE
    IF NOT EXISTS vaultkeeps(
        id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
        creatorId VARCHAR(250) NOT NULL,
        vaultId INT NOT NUll,
        keepId INT NOT NULL,
        FOREIGN KEY(vaultId) REFERENCES vaults (id) ON DELETE CASCADE,
        FOREIGN KEY(keepId) REFERENCES keeps (id) ON DELETE CASCADE
    ) default charset utf8;

CREATE TABLE
    IF NOT EXISTS vaults(
        id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
        creatorId VARCHAR(300) NOT NULL,
        name VARCHAR(50) NOT NULL,
        description VARCHAR(250) NOT NULL,
        img VARCHAR(250) NOT NULL DEFAULT "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSwWJlhefBA3SXAlCNgOq_L7CAlPtSCCME0mg&usqp=CAU",
        isPrivate bool NOT NULL DEFAULT false,
        FOREIGN KEY(creatorId) REFERENCES accounts(id) ON DELETE CASCADE
    ) default charset utf8;