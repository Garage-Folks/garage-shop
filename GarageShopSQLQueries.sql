CREATE TABLE LocationConstraint (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Description nVARCHAR(2000) NOT NULL
);

CREATE TABLE Location (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Area nVARCHAR(10) NOT NULL,
    Locus nVARCHAR(25) NOT NULL
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
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT ''
);

CREATE TABLE Brand (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT ''
);

CREATE TABLE Lumber (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    SpeciesWoodID uniqueidentifier,
    Length FLOAT NOT NULL,
    Width FLOAT NOT NULL,
    Thickness FLOAT NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE Log (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    SpeciesWoodID uniqueidentifier,
    Length FLOAT NOT NULL,
    Diameter FLOAT NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE MiscWood (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    SpeciesWoodID uniqueidentifier,
    SpeciesDesc nVARCHAR(100) NOT NULL DEFAULT '',
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (SpeciesWoodID) REFERENCES SpeciesWood(ID)
);

CREATE TABLE SheetMaterial (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    FOREIGN KEY (LocationID) REFERENCES Location(ID)
);

CREATE TABLE Oil (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    Dry VARCHAR(10) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Varnish (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Paint (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE MiscFinishProduct (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    MaterialType nVARCHAR(50) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Tool (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    ToolType nVARCHAR(50) NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Location(ID),
    FOREIGN KEY (BrandID) REFERENCES Brand(ID)
);

CREATE TABLE Glue (
    ID uniqueidentifier DEFAULT (NEWID()) PRIMARY KEY,
    LocationID uniqueidentifier,
    Name nVARCHAR(50) NOT NULL,
    Notes nVARCHAR(2000) NOT NULL DEFAULT '',
    Qty INT DEFAULT 1,
    LinkImg1 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg2 nVARCHAR(MAX) NOT NULL DEFAULT '',
    LinkImg3 nVARCHAR(MAX) NOT NULL DEFAULT '',
    BrandID uniqueidentifier,
    GlueType nVARCHAR(50) NOT NULL,
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
