ALTER TABLE characters ADD COLUMN duelWins INT DEFAULT 0;
ALTER TABLE characters ADD COLUMN duelLosses INT DEFAULT 0;


-- Create the duels table:
CREATE TABLE duels (
    duelId BIGINT AUTO_INCREMENT PRIMARY KEY,
    winnerId BIGINT NOT NULL,
    loserId BIGINT NOT NULL,
    duelTime DATETIME NOT NULL,
    FOREIGN KEY (winnerId) REFERENCES characters(characterId),
    FOREIGN KEY (loserId) REFERENCES characters(characterId)
);