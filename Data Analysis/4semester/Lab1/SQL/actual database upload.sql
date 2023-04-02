DROP TABLE IF EXISTS public.dim_country;
CREATE TABLE public.dim_country
(
    country                  VARCHAR(255) PRIMARY KEY,
    population               BIGINT,
    yearly_population_factor FLOAT,
    population_world_share   FLOAT,
    users_amount             BIGINT
);
INSERT INTO public.dim_country (country, population, yearly_population_factor, population_world_share, users_amount)
SELECT country,
       population_2020,
       yearly_change,
       world_share,
       users_amount
FROM staging_area.country_info;

DROP TABLE IF EXISTS public.dim_category;
CREATE TABLE public.dim_category
(
    id             INTEGER PRIMARY KEY,
    category_title VARCHAR(255)
);

INSERT INTO public.dim_category (id, category_title)
SELECT CAST(id AS INTEGER), title
FROM staging_area.video_category_info;

-- CREATE TABLE public.dim_channel AS
-- SELECT id,
--        youtuber    AS name,
--        subscribers AS subscriber_count,
--        video_views AS total_views,
--        video_count,
--        category    AS category_temp,
--        started     AS creation_year,
--        username,
--        youtube_url,
--        audience_country,
--        avg_likes,
--        avg_comments
-- FROM staging_area.channel_info;

DROP TABLE IF EXISTS public.dim_channel;
CREATE TABLE public.dim_channel
(
    id                  VARCHAR PRIMARY KEY,
    name                VARCHAR(255),
    subscriber_count    BIGINT,
    total_views         BIGINT,
    video_count         BIGINT,
    creation_year       INTEGER,
    username            VARCHAR(255),
    youtube_url         VARCHAR(255),
    avg_likes           FLOAT,
    avg_comments        FLOAT,
    audience_country_id VARCHAR(255) REFERENCES dim_country (country),
    category_id         INTEGER REFERENCES dim_category (id)
);

INSERT INTO public.dim_channel (id, name, subscriber_count, total_views, video_count, creation_year,
                                username, youtube_url, avg_likes, avg_comments, audience_country_id, category_id)
SELECT ci.id,
       youtuber,
       subscribers,
       video_views,
       video_count,
       started,
       username,
       youtube_url,
       avg_likes,
       avg_comments,
       audience_country,
       dc.id
FROM staging_area.channel_info AS ci
         JOIN dim_category AS dc ON ci.category = dc.category_title
WHERE ci.audience_country IN (SELECT country FROM dim_country);

DROP TABLE IF EXISTS public.video;
CREATE TABLE public.video
(
    id                   VARCHAR(255) PRIMARY KEY,
    date                 DATE         NOT NULL,
    title                VARCHAR(255) NOT NULL,
    description          TEXT,
    url                  VARCHAR(255) NOT NULL,
    tags                 VARCHAR(255)[],
    defaultAudioLanguage VARCHAR(255),
    duration             INTERVAL     NOT NULL,
    definition           VARCHAR      NOT NULL,
    viewCount            VARCHAR(255) NOT NULL,
    likeCount            VARCHAR(255) NOT NULL,
    commentCount         VARCHAR(255) NOT NULL,
    channel_id           VARCHAR(255) REFERENCES dim_channel (id),
    category_id          INTEGER REFERENCES dim_category (id)
);

INSERT INTO public.video (id,
                          date,
                          title,
                          description,
                          url,
                          tags,
                          defaultAudioLanguage,
                          duration,
                          definition,
                          viewCount,
                          likeCount,
                          commentCount,
                          channel_id,
                          category_id)
SELECT id,
       date,
       title,
       description,
       url,
       tags,
       defaultAudioLanguage,
       duration,
       definition,
       viewCount,
       likeCount,
       commentCount,
       channelId,
       CAST(categoryId AS INTEGER)
FROM staging_area.video_info
WHERE channelid IN (SELECT id FROM dim_channel);

DROP TABLE IF EXISTS public.dim_comment;
CREATE TABLE public.dim_comment
(
    id                SERIAL PRIMARY KEY,
    text              VARCHAR,
    author_name       VARCHAR,
    author_id         VARCHAR,
    like_count        INTEGER,
    published_date    DATE,
    total_reply_count INTEGER,
    video_id          VARCHAR REFERENCES public.video (id)
);

INSERT INTO public.dim_comment (text, author_name, author_id, like_count, published_date, total_reply_count, video_id)
SELECT
       textdisplay,
       authordisplayname,
       authorid,
       CAST(likecount AS INTEGER),
       publishedat,
       CAST(totalreplycount AS INTEGER),
       videoid
FROM
    staging_area.comment_info
WHERE videoid IN (SELECT id FROM video);


