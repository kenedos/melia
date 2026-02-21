DROP TABLE IF EXISTS password_reset_tokens;
CREATE TABLE IF NOT EXISTS password_reset_tokens (
	id INT AUTO_INCREMENT PRIMARY KEY,
	email VARCHAR(255) NOT NULL,
	token_hash VARCHAR(64) NOT NULL,
	expires_at DATETIME NOT NULL,
	used_at DATETIME NULL,
	created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
	INDEX idx_email (email),
	INDEX idx_expires (expires_at)
);