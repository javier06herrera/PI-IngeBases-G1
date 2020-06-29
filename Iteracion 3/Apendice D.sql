/*ANEXO C: Cuatro consultas avanzadas utilizadas*/

--Consulta 1 Consulta usada para el proceso de generar reporte)

SELECT H.skillName As 'Value', (Count(*)) As 'Count'
FROM CommunityMember C, HAS_SKILL H, Skill S
WHERE C.email = H.email  
AND H.category = S.subjectCategory
AND H.skillName = subjectSkillName
Group By H.skillName

--Consulta 2 SQL Avanzado para un procedimiento almacenado)

--CREATE FUNCTION PIF_getTopics(@articleId int)
--RETURNS varchar(max)
--AS
--BEGIN
--declare @result varchar(max)
--set @result =''
--select @result = @result + I.topicName + ', '
--from Article A
--join INVOLVES I on A.articleId = I.articleId
--where A.articleId = @articleId
--RETURN @result
--END

SELECT A.*, dbo.PIF_getTopics(A.articleId) as topicName
FROM Article A
ORDER BY publishDate DESC

--Consulta 3: SQL Avanzado usado para un trigger)

/* CONTEXTO
--DECLARE @articleId int
--DECLARE @newLikeBalance int
--DECLARE @oldLikeBalance int
--select @articleId=I.articleId, @newLikeBalance = I.likesCount - I.dislikesCount
--from inserted I
--select @oldLikeBalance = D.likeBalance
--from deleted D
*/

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

		
--Consulta 4: SQL Avanzado usado para en el proceso de revision de articulo)
SELECT * 
FROM INVOLVES                                 
WHERE  articleId 
IN (
		SELECT articleId
		FROM   REVIEWS
        WHERE  email = @email 
		AND    state = 'not reviewed') 
ORDER BY articleId