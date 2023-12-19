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

DROP TABLE IF EXISTS public.dim_date;
CREATE TABLE public.dim_date
(
    id    SERIAL PRIMARY KEY,
    year  INT,
    month INT,
    day   INT
);
INSERT INTO public.dim_date (year, month, day)
SELECT EXTRACT(YEAR FROM date)  AS year,
       EXTRACT(MONTH FROM date) AS month,
       EXTRACT(DAY FROM date)   AS day
FROM (SELECT DISTINCT date
      FROM staging_area.video_info
      UNION
      SELECT DISTINCT publishedat
      FROM staging_area.comment_info
      UNION
      SELECT DISTINCT MAKE_DATE(started, 1, 1)
      FROM staging_area.channel_info) AS distinct_dates;

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

DROP TABLE IF EXISTS public.channel;
CREATE TABLE public.channel
(
    id                  VARCHAR PRIMARY KEY,
    name                VARCHAR(255),
    subscriber_count    BIGINT,
    total_views         BIGINT,
    video_count         BIGINT,
    username            VARCHAR(255),
    youtube_url         VARCHAR(255),
    avg_likes           FLOAT,
    avg_comments        FLOAT,
    audience_country_id VARCHAR(255) REFERENCES dim_country (country),
    category_id         INTEGER REFERENCES dim_category (id),
    creation_date_id    INTEGER REFERENCES dim_date (id)
);

INSERT INTO public.channel (id, name, subscriber_count, total_views, video_count,
                            username, youtube_url, avg_likes, avg_comments, audience_country_id, category_id,
                            creation_date_id)
SELECT ci.id,
       youtuber,
       subscribers,
       video_views,
       video_count,
       username,
       youtube_url,
       avg_likes,
       avg_comments,
       audience_country,
       dc.id,
       dd.id
FROM staging_area.channel_info AS ci
         JOIN dim_category AS dc ON ci.category = dc.category_title
         JOIN dim_date AS dd ON ci.started = dd.year AND
                                dd.month = 1 AND
                                dd.day = 1
WHERE ci.audience_country IN (SELECT country FROM dim_country);

DROP TABLE IF EXISTS public.video;
CREATE TABLE public.video
(
    id                   VARCHAR(255) PRIMARY KEY,
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
    channel_id           VARCHAR(255) REFERENCES channel (id),
    category_id          INTEGER REFERENCES dim_category (id),
    published_date_id    INTEGER REFERENCES dim_date (id),
    scd VARCHAR(255) REFERENCES video (id)
);

INSERT INTO public.video (id,
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
                          category_id,
                          published_date_id)
SELECT vi.id,
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
       CAST(categoryId AS INTEGER),
       dd.id
FROM staging_area.video_info AS vi
         JOIN dim_date dd ON EXTRACT(YEAR FROM vi.date) = dd.year AND
                             EXTRACT(MONTH FROM vi.date) = dd.month AND
                             EXTRACT(DAY FROM vi.date) = dd.day
WHERE channelid IN (SELECT id FROM channel);

DROP TABLE IF EXISTS public.comment;
CREATE TABLE public.comment
(
    id                SERIAL PRIMARY KEY,
    text              VARCHAR,
    author_name       VARCHAR,
    author_id         VARCHAR,
    like_count        INTEGER,
    total_reply_count INTEGER,
    video_id          VARCHAR REFERENCES public.video (id),
    published_date_id    INTEGER REFERENCES dim_date (id)
);

INSERT INTO public.comment (text, author_name, author_id, like_count, total_reply_count, video_id, published_date_id)
SELECT textdisplay,
       authordisplayname,
       authorid,
       CAST(likecount AS INTEGER),
       CAST(totalreplycount AS INTEGER),
       videoid,
       dd.id
FROM staging_area.comment_info AS ci JOIN dim_date AS dd ON EXTRACT(YEAR FROM ci.publishedat) = dd.year AND
                            EXTRACT(MONTH FROM ci.publishedat) = dd.month AND
                            EXTRACT(DAY FROM ci.publishedat) = dd.day
WHERE videoid IN (SELECT id FROM video);


