--Query number one
SELECT *
FROM Article
ORDER BY publishDate DESC

--Query number two
SELECT *
FROM Question
WHERE faq = 'posted' 


--Query number three
--The @ means that informations comes from the model of the web app
SELECT A.*, I.topicName
FROM Article A
JOIN INVOLVES I ON A.articleId = I.articleId
WHERE I.topicName = @topicName