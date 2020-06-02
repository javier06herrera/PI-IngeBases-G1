
--Primera consulta simple
SELECT *
FROM Article
ORDER BY publishDate DESC

--Seguna consulta simple
SELECT *
FROM Question
WHERE faq = 'posted' 

--Tercera consulta simple
--El @ indica que el atributo entra como parametro desde el modelo
SELECT A.*, I.topicName
FROM Article A
JOIN INVOLVES I ON A.articleId = I.articleId
WHERE I.topicName = @topicName