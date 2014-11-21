-- # Input parameters # --
DECLARE @Value table
(
    value float
);
INSERT INTO @Value
VALUES
    (0.5)
  , (1)
  , (1.5)
  , (7)
  , (7.5);

DECLARE @X table
(
    x float
);
INSERT INTO @X
VALUES
    (1)
  , (2)
  , (3)
  , (4)
  , (5)
  , (6)
  , (7);

DECLARE @Y table
(
    y float
);
INSERT INTO @Y
VALUES
    (2)
  , (4)
  , (6)
  , (8)
  , (10)
  , (12)
  , (14);

-- # Core # --
DECLARE @MinX float, @MaxX float, @MinY float, @MaxY float;

SELECT @MinX = min(X.x)
     , @MaxX = max(X.x)
FROM
    @X X;

SELECT @MinY = min(Y.y)
     , @MaxY = max(Y.y)
FROM
    @Y Y;

WITH T AS
(
SELECT X.RowNumber
     , X.x
     , Y.y
FROM
    (SELECT row_number() OVER (ORDER BY X.x) RowNumber
          , X.x
     FROM
         @X X) X
    INNER JOIN (SELECT row_number() OVER (ORDER BY Y.y) RowNumber
                     , Y.y
                FROM
                    @Y Y) Y
        ON Y.RowNumber = X.RowNumber
)
SELECT V.value
     , CASE
           WHEN V.value < @MinX THEN
               @MinY
           WHEN V.value > @MaxX THEN
               @MaxY
           ELSE
               lowerT.y + (upperT.y - lowerT.y) / (upperT.x - lowerT.x) * (V.value - lowerT.x)
       END
FROM
    @Value V
    CROSS JOIN T lowerT
    LEFT JOIN T upperT
        ON upperT.RowNumber = lowerT.RowNumber + 1
WHERE
    (lowerT.x <= V.value
    AND upperT.x >= V.value)
    OR (upperT.RowNumber IS NULL
    AND (V.value > @MaxX
    OR V.value < @MinX));