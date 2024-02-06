CREATE TABLE LocationConstraint (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Description nVARCHAR(2000)
);

CREATE TABLE Location (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Area nVARCHAR(10),
    Locus nVARCHAR(25)
);

CREATE TABLE ConstraintOfLocation (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    ConstraintID uniqueidentifier,
    LocationID uniqueidentifier,
    FOREIGN KEY (ConstraintID) REFERENCES LocationConstraint(ID),
    FOREIGN KEY (LocationID) REFERENCES Location(ID)
);

CREATE TABLE SpeciesWood (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000)
);

CREATE TABLE Brand (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000)
);

CREATE TABLE Lumber (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(100),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    SpeciesWoodID uniqueidentifier,
    Length FLOAT,
    Width FLOAT,
    Thickness FLOAT,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE Log (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(100),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    SpeciesWoodID uniqueidentifier,
    Length FLOAT,
    Diameter FLOAT,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE MiscWood (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(100),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    SpeciesWoodID uniqueidentifier,
    SpeciesDesc nVARCHAR(100),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE SheetMaterial (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    FOREIGN KEY (LocationID) REFERENCES Location(ID)
);

CREATE TABLE Oil (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    Dry VARCHAR(10),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Varnish (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Paint (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE MiscFinishProduct (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Tool (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    ToolType nVARCHAR(50),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Glue (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50),
    Notes nVARCHAR(2000),
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX),
    LinkImg2 nVARCHAR(MAX),
    LinkImg3 nVARCHAR(MAX),
    BrandID uniqueidentifier,
    GlueType nVARCHAR(50),
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);


CREATE TABLE AuthorizedUser (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Username nVARCHAR(50) UNIQUE,
    Password nVARCHAR(MAX),
    Email nVARCHAR(100),
    Notes nVARCHAR(2000),
    Role nVARCHAR(10)
);
