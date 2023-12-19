CREATE SCHEMA staging_area;


DROP TABLE IF EXISTS staging_area.video_info;
CREATE TABLE staging_area.video_info
(
    id                   VARCHAR(255) PRIMARY KEY,
    date                 DATE         NOT NULL,
    channelId            VARCHAR(255) NOT NULL,
    title                VARCHAR(255) NOT NULL,
    description          TEXT,
    url                  VARCHAR(255) NOT NULL,
    channelTitle         VARCHAR(255) NOT NULL,
    tags                 VARCHAR(255)[],
    categoryId           VARCHAR(255) NOT NULL,
    defaultAudioLanguage VARCHAR(255),
    duration             INTERVAL     NOT NULL,
    definition           VARCHAR(255) NOT NULL,
    viewCount            VARCHAR(255) NOT NULL,
    likeCount            VARCHAR(255) NOT NULL,
    commentCount         VARCHAR(255) NOT NULL
);
-- Upload the data from the CSV file
COPY staging_area.video_info (date,
                              id,
                              channelid,
                              title,
                              description,
                              url,
                              channeltitle,
                              tags,
                              categoryid,
                              defaultaudiolanguage,
                              duration,
                              definition,
                              viewcount,
                              likecount,
                              commentcount)
    FROM 'C:\Users\vikto\Workspace\GitRepos\KPI\Data Analysis\4semester\Lab1\data\cleaned\merged_video_data.csv'
    WITH (FORMAT CSV, DELIMITER ',', QUOTE '"', HEADER TRUE);


DROP TABLE IF EXISTS staging_area.channel_info;
CREATE TABLE staging_area.channel_info
(
    youtuber         VARCHAR NOT NULL,
    subscribers      BIGINT  NOT NULL,
    video_views      BIGINT  NOT NULL,
    video_count      INT     NOT NULL,
    category         VARCHAR NOT NULL,
    started          INT     NOT NULL,
    username         VARCHAR NOT NULL,
    youtube_url      VARCHAR NOT NULL,
    audience_country VARCHAR NOT NULL,
    avg_likes        FLOAT   NOT NULL,
    avg_comments     FLOAT   NOT NULL,
    id               VARCHAR PRIMARY KEY
);

COPY staging_area.channel_info (youtuber,
                                subscribers,
                                video_views,
                                video_count,
                                category,
                                started,
                                username,
                                youtube_url,
                                audience_country,
                                avg_likes,
                                avg_comments,
                                id)
    FROM 'C:\Users\vikto\Workspace\GitRepos\KPI\Data Analysis\4semester\Lab1\data\cleaned\merged_channel_data.csv'
    WITH (FORMAT CSV, DELIMITER ',', QUOTE '"', HEADER TRUE);


DROP TABLE IF EXISTS staging_area.comment_info;
CREATE TABLE staging_area.comment_info
(
    id                SERIAL PRIMARY KEY,
    videoId           VARCHAR,
    textDisplay       VARCHAR,
    authorDisplayName VARCHAR,
    authorId          VARCHAR,
    likeCount         VARCHAR,
    publishedAt       DATE,
    totalReplyCount   VARCHAR
);

COPY staging_area.comment_info (videoId, textDisplay, authorDisplayName, authorId, likeCount, publishedAt,
                                totalReplyCount)
    FROM 'C:\Users\vikto\Workspace\GitRepos\KPI\Data Analysis\4semester\Lab1\data\extracted\comments.csv'
    WITH (FORMAT CSV, DELIMITER ',', QUOTE '"', HEADER TRUE);

DROP TABLE IF EXISTS staging_area.video_category_info;
CREATE TABLE staging_area.video_category_info
(
    id         VARCHAR PRIMARY KEY,
    title      TEXT NOT NULL,
    channel_id TEXT NOT NULL
);

COPY staging_area.video_category_info (id, title, channel_id)
    FROM 'C:\Users\vikto\Workspace\GitRepos\KPI\Data Analysis\4semester\Lab1\data\extracted\video_categories.csv'
    DELIMITER ','
    CSV HEADER;

DROP TABLE IF EXISTS staging_area.country_info;
CREATE TABLE staging_area.country_info (
    country VARCHAR(255) PRIMARY KEY,
    population_2020 BIGINT,
    yearly_change FLOAT,
    world_share FLOAT,
    users_amount BIGINT
);

COPY staging_area.country_info (population_2020, yearly_change, world_share, country, users_amount)
FROM 'C:\Users\vikto\Workspace\GitRepos\KPI\Data Analysis\4semester\Lab1\data\cleaned\merged_country_data.csv' DELIMITER ',' CSV HEADER;

