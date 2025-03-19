IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'FunBooksAndVideos')
BEGIN
    CREATE DATABASE FunBooksAndVideos;
END;