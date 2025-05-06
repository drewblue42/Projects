from fastapi import HTTPException
from fastapi.responses import JSONResponse
from pydantic import BaseModel

class ClientErrorResponse(BaseModel):
    error: str
    message: str

class ExceptionModel(HTTPException):
    def __init__(self, status_code: int, error: str, message: str):
        super().__init__(status_code=status_code, detail={"error": error, "message": message})
    
    def response(self) -> JSONResponse:
        return JSONResponse(
            status_code=self.status_code,
            content=ClientErrorResponse(
                error=self.detail["error"],
                message=self.detail["message"]
            ).model_dump()
        )