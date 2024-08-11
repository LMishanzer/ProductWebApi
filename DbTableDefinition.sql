create table Products (
    id uniqueidentifier primary key,
    [name] nvarchar(100) not null,
    imgUri nvarchar(1000) not null,
    price decimal(18, 2) not null,
    [description] nvarchar(3000) not null
);
