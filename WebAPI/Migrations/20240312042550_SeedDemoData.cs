﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    public partial class SeedDemoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @UserID as INT,
                    @CountryId AS INT;
                --------------------------
                --Create User
                --------------------------
                IF exists (select Id from Users where Username='SanjayDangi1')
                --insert into Users(Username,Password, PasswordKey,DateModified,UserModified)
                -- select 'Demo',
                -- 0x4D5544D09B8319B423F6D4E054360D5289B57A98781A66B276E00C57919FDCD599BF45623D48CC81F535748F560AF0F70C8C7F3B4C3DB672562B5DD0E5E7C297,
                -- 0x44A0BD5BFD689DF399346200A1117C33BEDF5869C17A7CB3DC6D8598A93845DB333B379AA90931D8D4E5F2CC7B1A4A96A7DB71B186DBCDCDC53B0A95440E4EDD7473668627970FBD9BB0BA17530CCAB2D9446A1902BD6AC12FE691FE09DD78A43398B89111056145843060026A414FFA8C5E75B474E187AD753D2872038D9FDD,
                -- getdate(),
                -- 0

                SET @UserID = (select id from Users where Username='SanjayDangi1')

                --------------------------
                --Seed Property Types
                --------------------------
                IF not exists (select name from PropertyTypes where Name='House')
                insert into PropertyTypes(Name,DateModified,UserModified)
                select 'House', GETDATE(),@UserID

                IF not exists (select name from PropertyTypes where Name='Apartment')
                insert into PropertyTypes(Name,DateModified,UserModified)
                select 'Apartment', GETDATE(),@UserID
                    
                IF not exists (select name from PropertyTypes where Name='Duplex')
                insert into PropertyTypes(Name,DateModified,UserModified)
                select 'Duplex', GETDATE(),@UserID


                --------------------------
                --Seed Furnishing Types
                --------------------------
                IF not exists (select name from FurnishingTypes where Name='Fully')
                insert into FurnishingTypes(Name, DateModified,UserModified)
                select 'Fully', GETDATE(),@UserID
                    
                IF not exists (select name from FurnishingTypes where Name='Semi')
                insert into FurnishingTypes(Name, DateModified,UserModified)
                select 'Semi', GETDATE(),@UserID
                    
                IF not exists (select name from FurnishingTypes where Name='Unfurnished')
                insert into FurnishingTypes(Name, DateModified,UserModified)
                select 'Unfurnished', GETDATE(),@UserID

                --------------------------
                --Seed Countries
                --------------------------
                IF NOT EXISTS(SELECT Name FROM Country)
                INSERT INTO Country(Name, UserModified, DateModified)
                SELECT 'USA', @UserID, GETDATE()
                UNION
                SELECT 'CANADA', @UserID, GETDATE()
                UNION
                SELECT 'RUSSIA', @UserID, GETDATE()
                UNION
                SELECT 'INDIA', @UserID, GETDATE() 

                SET @CountryId = (SELECT Id FROM Country WHERE Name = 'INDIA')


                --------------------------
                --Seed Cities
                --------------------------
                IF not exists (select top 1 id from Cities)
                Insert into Cities(Name, CountryId, UserModified, DateModified)
                
                select 'New Delhi',@CountryId,@UserID,getdate()
                union
                select 'Bangalore',@CountryId,@UserID,getdate()
                union
                select 'Chandigarh',@CountryId,@UserID,getdate()
                union
                select 'Gurgaon',@CountryId,@UserID,getdate()

                --------------------------
                --Seed Properties
                --------------------------
                --Seed property for sell
                IF not exists (select top 1 name from Properties where Name='White House Demo')
                insert into Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuildArea,CarpertArea,Address,
                Address2,CityId,FloorNo,TotalFloor,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,DateModified,UserModified)
                select 
                1, --Sell Rent
                'White House Demo', --Name
                (select Id from PropertyTypes where Name='Apartment'), --Property Type ID
                2, --BHK
                (select Id from FurnishingTypes where Name='Fully'), --Furnishing Type ID
                1800, --Price
                1400, --Built Area
                900, --Carpet Area
                '6 Street', --Address
                'Golf Course Road', -- Address2
                (select top 1 Id from Cities), -- City ID
                3, -- Floor No
                3, --Total Floors
                1, --Ready to Move
                'East', --Main Entrance
                0, --Security
                1, --Gated
                300, -- Maintenance
                '2019-01-01', -- Establishment or Posession on
                0, --Age
                'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
                GETDATE(), --Posted on
                @UserID, --Posted by
                GETDATE(), --Last Updated on
                @UserID --Last Updated by

                ---------------------------
                --Seed property for rent
                ---------------------------
                IF not exists (select top 1 name from Properties where Name='Birla House Demo')
                insert into Properties(SellRent,Name,PropertyTypeId,BHK,FurnishingTypeId,Price,BuildArea,CarpertArea,Address,
                Address2,CityId,FloorNo,TotalFloor,ReadyToMove,MainEntrance,Security,Gated,Maintenance,EstPossessionOn,Age,Description,PostedOn,PostedBy,DateModified,UserModified)
                select 
                2, --Sell Rent
                'Birla House Demo', --Name
                (select Id from PropertyTypes where Name='Apartment'), --Property Type ID
                2, --BHK
                (select Id from FurnishingTypes where Name='Fully'), --Furnishing Type ID
                1800, --Price
                1400, --Built Area
                900, --Carpet Area
                '6 Street', --Address
                'Golf Course Road', -- Address2
                (select top 1 Id from Cities), -- City ID
                3, -- Floor No
                3, --Total Floors
                1, --Ready to Move
                'East', --Main Entrance
                0, --Security
                1, --Gated
                300, -- Maintenance
                '2019-01-01', -- Establishment or Posession on
                0, --Age
                'Well Maintained builder floor available for rent at prime location. # property features- - 5 mins away from metro station - Gated community - 24*7 security. # property includes- - Big rooms (Cross ventilation & proper sunlight) - 
                Drawing and dining area - Washrooms - Balcony - Modular kitchen - Near gym, market, temple and park - Easy commuting to major destination. Feel free to call With Query.', --Description
                GETDATE(), --Posted on
                @UserID, --Posted by
                GETDATE(), --Last Updated on
                @UserID --Last Updated by

            
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.Sql(@"
                --------------------
                --Seeding Down
                --------------------
                DECLARE @UserID as int
                SET @UserID = (select id from Users where Username='SanjayDangi1')
                delete from Users where Username='SanjayDangi1'
                delete from PropertyTypes where UserModified=@UserID
                delete from FurnishingTypes where UserModified=@UserID
                delete from Cities where UserModified=@UserID
                delete from Properties where PostedBy=@UserId
                delete from Country Where UserModified=@UserID

                ");
        }
    }
}
