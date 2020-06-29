-------------------------------------------------------------------------------------------
--Funciones
CREATE FUNCTION PIF_getTopics(@articleId int)
RETURNS varchar(max)
AS
BEGIN
declare @result varchar(max)
set @result =''
select @result = @result + I.topicName + ', '
from Article A
join INVOLVES I on A.articleId = I.articleId
where A.articleId = @articleId
RETURN @result
END
--------------------------------------------------------------------------------------------
--Procedimientos
---Primero
CREATE PROC PISP_getArticles AS 
SELECT A.*, dbo.PIF_getTopics(A.articleId) as topicName
FROM Article A
ORDER BY publishDate DESC

CREATE PROC PISP_getMemberProfile @email varchar(25) AS
DECLARE @articleCount int 
SELECT @articleCount = COUNT(*)
FROM WRITES W
WHERE W.email = @email

SELECT  C.*, @articleCount AS 'articleCount'
FROM CommunityMember C
WHERE C.email = @email

---Segundo
CREATE PROC PISP_getMemeberArticle @email varchar(25) AS 
SELECT A.*, dbo.PIF_getTopics(A.articleId) AS topicName
FROM Article A
JOIN WRITES W ON A.articleId = W.articleId
WHERE W.email = @email
ORDER BY publishDate DESC

----------------------------------------------------------------------------------------------
--Disparadores
---Primero
CREATE TRIGGER PIT_totalCountUpdate
ON Article
FOR  UPDATE
AS

DECLARE @articleId int
DECLARE @newLikeBalance int
DECLARE @oldLikeBalance int
select @articleId=I.articleId, @newLikeBalance = I.likesCount - I.dislikesCount
from inserted I
select @oldLikeBalance = D.likeBalance
from deleted D

IF UPDATE(likesCount) OR UPDATE(dislikesCount)
BEGIN
	UPDATE Article
	SET likeBalance = likesCount-dislikesCount
	WHERE articleId = @articleId 

	UPDATE	CommunityMember
	SET Points = Points + @newLikeBalance - @oldLikeBalance
	WHERE email in (
		SELECT W.email
		FROM WRITES W
		WHERE W.articleId = @articleId)
END

---Segundo
CREATE TRIGGER PIT_viewsMeritUpdate
ON Article
FOR  UPDATE
AS
DECLARE @articleId int
SELECT @articleId = I.articleId
FROM inserted I

IF UPDATE(accessCount)
BEGIN
	UPDATE CommunityMember
	SET points = points + 1
	WHERE email IN (
			SELECT W.email
			FROM WRITES W
			WHERE W.articleId = @articleId)
END 

