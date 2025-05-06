from fastapi.responses import JSONResponse
from fastapi import HTTPException
from backend.models.exceptions import ExceptionModel

class EntityNotFound(ExceptionModel):
    def __init__(self, entity: str, id):
        super().__init__(
            status_code=404,
            error="entity_not_found",
            message=f"Unable to find {entity} with id={id}"
        )

class InvalidCredentials(ExceptionModel):
    def __init__(self):
        super().__init__(
            status_code=401,
            error="invalid_credentials",
            message="Authentication failed: invalid username or password"
        )

class NotAuthenticated(ExceptionModel):
    def __init__(self):
        super().__init__(
            status_code=403,
            error="authentication_required",
            message="Not authenticated"
        )

class ExpiredAccessToken(ExceptionModel):
    def __init__(self):
        super().__init__(
            status_code=403,
            error="expired_access_token",
            message="Authentication failed: expired access token"
        )

class InvalidAccessToken(ExceptionModel):
    def __init__(self):
        super().__init__(
            status_code=403,
            error="invalid_access_token",
            message="Authentication failed: invalid access token"
        )

class DuplicateEntityError(ExceptionModel):
    def __init__(self, field: str, value: str):
        super().__init__(
            status_code=422,
            error="duplicate_entity_value",
            message=f"Duplicate value: account with {field}={value} already exists"
        )

