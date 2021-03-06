USE [UPAXER]
GO
/****** Object:  Table [dbo].[TAUser]    Script Date: 25/05/2020 01:18:13 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TAUser](
	[Id] [int] NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Apaterno] [varchar](30) NOT NULL,
	[Amaterno] [varchar](30) NOT NULL,
	[NombreUsuario] [varchar](10) NOT NULL,
	[Contraseña] [varchar](10) NOT NULL,
	[Correo] [varchar](30) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PKTAUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[PRCTAUser]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[PRCTAUser] (
			@Nombre							VARCHAR(30),
			@Apaterno						VARCHAR(30),
			@Amaterno						VARCHAR(30),
			@NombreUsuario					VARCHAR(10),
			@Contraseña						VARCHAR(10),
			@Correo							VARCHAR(30)
)
AS
  DECLARE	@vcMensaje						VARCHAR(255),
			@Id								INT,
			@Estado							BIT						= 1
			
  SET NOCOUNT ON
	BEGIN
		BEGIN TRAN INSERTTAUser
			SELECT @Id = 1 + ISNULL(MAX(Id), 0)
			FROM TAUser WITH (ROWLOCK, UPDLOCK)

			INSERT INTO TAUser(
      			Id,
      			Nombre,
      			Apaterno,
      			Amaterno,
      			NombreUsuario,
      			Contraseña,
				Correo,	
				Estado	
			)
			VALUES		
			(
				@Id,
				@Nombre,
				@Apaterno,
				@Amaterno,
				@NombreUsuario,
				@Contraseña,
				@Correo,	
				@Estado	
			)
			   IF (@@ERROR <> 0)
				BEGIN
					SET @vcMensaje = 'ERROR AL TRATAR DE HACER EL INSERT A TAUser'
					GOTO CtrlErrores
				END
		COMMIT TRAN INSERTTAUser
	END
  SET NOCOUNT OFF
  RETURN 0
-----------------------------------------------------------------------------------------    
--- Manejo de Errores    
-----------------------------------------------------------------------------------------    
CtrlErrores:
  SET NOCOUNT OFF
      IF @@TRANCOUNT > 0  
      BEGIN  
          ROLLBACK TRAN INSERTTAUser
      END  
  SET @vcMensaje = 'PRCTAUser:' + ISNULL(@vcMensaje, '')
  RAISERROR (@vcMensaje, 18, 1)
  RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[PRDTAUser]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PRDTAUser] (
			@Id								INT
)
AS
  DECLARE	@vcMensaje								VARCHAR(255),
			@Estado									BIT						= 0			
  
  SET NOCOUNT ON
	BEGIN
		BEGIN TRAN DELETETAUser
			UPDATE TAUser
			SET 
			Estado  =	@Estado
			WHERE 
			Id	=	@Id
				  IF (@@ERROR <> 0)
				BEGIN
					SET @vcMensaje = 'ERROR AL TRATAR DE HACER EL DELETE A TAUser'
					GOTO CtrlErrores
				END
		COMMIT TRAN DELETETAUser
	END
  SET NOCOUNT OFF
  RETURN 0
-----------------------------------------------------------------------------------------    
--- Manejo de Errores    
-----------------------------------------------------------------------------------------    
CtrlErrores:
  SET NOCOUNT OFF
      IF @@TRANCOUNT > 0  
      BEGIN  
          ROLLBACK TRAN DELETETAUser
      END  
  SET @vcMensaje = 'PRDTAUser:' + ISNULL(@vcMensaje, '')
  RAISERROR (@vcMensaje, 18, 1)
  RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[PRRLogin]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PRRLogin](
@Nombre							VARCHAR(30),
@Contraseña						VARCHAR(10)
)
AS  
  
  SET NOCOUNT ON
	BEGIN
	--IF EXISTS (SELECT * FROM TAUser WHERE NombreUsuario = @Nombre AND Contraseña = @Contraseña )
	--	BEGIN
			SELECT * FROM TAUser WHERE NombreUsuario = @Nombre AND Contraseña = @Contraseña AND Estado = 1
	--	END
	--ELSE
	--	BEGIN
	--		SELECT * FROM TAUser WHERE NombreUsuario = @Nombre AND Contraseña = @Contraseña
	--		--USUARIO NO EXISTE
	--	END
	END
  SET NOCOUNT OFF
		
GO
/****** Object:  StoredProcedure [dbo].[PRRRecover]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PRRRecover](
@Nombre							VARCHAR(30)
)
AS  
  
  SET NOCOUNT ON
	BEGIN
	SELECT * FROM TAUser WHERE NombreUsuario = @Nombre
	END
  SET NOCOUNT OFF
		
GO
/****** Object:  StoredProcedure [dbo].[PRRTAUser]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PRRTAUser]
AS  
  SET NOCOUNT ON
	BEGIN
		SELECT
      		Id,
      		Nombre,
      		Apaterno,
      		Amaterno,
      		NombreUsuario,
			Contraseña,
			Correo,	
			Estado	
		FROM TAUser WITH (NOLOCK)
		WHERE Estado = 1
	END
  SET NOCOUNT OFF
GO
/****** Object:  StoredProcedure [dbo].[PRUTAUser]    Script Date: 25/05/2020 01:18:14 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Responsable:      : Kevin Kiyoshi Orea Castellanos
-- Fecha:            : Marzo 2020  
---Descripcion:      : Procedimiento para Insertar en TAUser
---Aplicacion        : Base   
--------------------------------------------------------------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[PRUTAUser] (
			@Id								INT,
			@Nombre							VARCHAR(30),
			@Apaterno						VARCHAR(30),
			@Amaterno						VARCHAR(30),
			@NombreUsuario					VARCHAR(10),
			@Contraseña						VARCHAR(10),
			@Correo							VARCHAR(30)
)
AS
  DECLARE	@vcMensaje						VARCHAR(255)
  
  SET NOCOUNT ON
	BEGIN
		BEGIN TRAN UPDATETAUser
			UPDATE TAUser
			SET 
			Nombre			=	@Nombre,
			Apaterno		=	@Apaterno,
			Amaterno		=	@Amaterno,
			NombreUsuario	=	@NombreUsuario,
			Contraseña		=	@Contraseña,
			Correo			=	@Correo
			WHERE 
			Id = @Id
				  IF (@@ERROR <> 0)
				BEGIN
					SET @vcMensaje = 'ERROR AL TRATAR DE HACER EL UPDATE A TAUser'
					GOTO CtrlErrores
				END
		COMMIT TRAN UPDATETAUser
	END
  SET NOCOUNT OFF
 RETURN 0
-----------------------------------------------------------------------------------------    
--- Manejo de Errores    
-----------------------------------------------------------------------------------------    
CtrlErrores:
  SET NOCOUNT OFF
      IF @@TRANCOUNT > 0  
      BEGIN  
          ROLLBACK TRAN UPDATETAUser
      END  
  SET @vcMensaje = 'PRUTAUser:' + ISNULL(@vcMensaje, '')
  RAISERROR (@vcMensaje, 18, 1)
  RETURN -1
GO
