export interface paths {
    "/content-entries/{name}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    name: string;
                };
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": Record<string, never>[];
                        "application/json": Record<string, never>[];
                        "text/json": Record<string, never>[];
                    };
                };
            };
        };
        put?: never;
        post: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    name: string;
                };
                cookie?: never;
            };
            requestBody: {
                content: {
                    "application/json": Record<string, never>;
                    "text/json": Record<string, never>;
                    "application/*+json": Record<string, never>;
                };
            };
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": Record<string, never>;
                        "application/json": Record<string, never>;
                        "text/json": Record<string, never>;
                    };
                };
            };
        };
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/content-types": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: {
            parameters: {
                query?: {
                    kind?: string;
                };
                header?: never;
                path?: never;
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentTypeDto"][];
                        "application/json": components["schemas"]["ContentTypeDto"][];
                        "text/json": components["schemas"]["ContentTypeDto"][];
                    };
                };
            };
        };
        put?: never;
        post: {
            parameters: {
                query?: never;
                header?: never;
                path?: never;
                cookie?: never;
            };
            requestBody: {
                content: {
                    "application/json": components["schemas"]["CreateContentTypeCommand"];
                    "text/json": components["schemas"]["CreateContentTypeCommand"];
                    "application/*+json": components["schemas"]["CreateContentTypeCommand"];
                };
            };
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentTypeDto"];
                        "application/json": components["schemas"]["ContentTypeDto"];
                        "text/json": components["schemas"]["ContentTypeDto"];
                    };
                };
            };
        };
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/content-types/{name}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    name: string;
                };
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentTypeDto"];
                        "application/json": components["schemas"]["ContentTypeDto"];
                        "text/json": components["schemas"]["ContentTypeDto"];
                    };
                };
            };
        };
        put?: never;
        post?: never;
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/content-types/{id}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post?: never;
        delete: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    id: string;
                };
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentTypeDto"];
                        "application/json": components["schemas"]["ContentTypeDto"];
                        "text/json": components["schemas"]["ContentTypeDto"];
                    };
                };
            };
        };
        options?: never;
        head?: never;
        patch: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    id: string;
                };
                cookie?: never;
            };
            requestBody: {
                content: {
                    "application/json": components["schemas"]["UpdateContentTypeDto"];
                    "text/json": components["schemas"]["UpdateContentTypeDto"];
                    "application/*+json": components["schemas"]["UpdateContentTypeDto"];
                };
            };
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentTypeDto"];
                        "application/json": components["schemas"]["ContentTypeDto"];
                        "text/json": components["schemas"]["ContentTypeDto"];
                    };
                };
            };
        };
        trace?: never;
    };
    "/content-types/{id}/fields": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    id: string;
                };
                cookie?: never;
            };
            requestBody: {
                content: {
                    "application/json": components["schemas"]["CreateContentFieldDto"];
                    "text/json": components["schemas"]["CreateContentFieldDto"];
                    "application/*+json": components["schemas"]["CreateContentFieldDto"];
                };
            };
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentFieldDto"];
                        "application/json": components["schemas"]["ContentFieldDto"];
                        "text/json": components["schemas"]["ContentFieldDto"];
                    };
                };
            };
        };
        delete?: never;
        options?: never;
        head?: never;
        patch?: never;
        trace?: never;
    };
    "/content-types/{id}/fields/{fieldId}": {
        parameters: {
            query?: never;
            header?: never;
            path?: never;
            cookie?: never;
        };
        get?: never;
        put?: never;
        post?: never;
        delete: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    id: string;
                    fieldId: string;
                };
                cookie?: never;
            };
            requestBody?: never;
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentFieldDto"];
                        "application/json": components["schemas"]["ContentFieldDto"];
                        "text/json": components["schemas"]["ContentFieldDto"];
                    };
                };
            };
        };
        options?: never;
        head?: never;
        patch: {
            parameters: {
                query?: never;
                header?: never;
                path: {
                    id: string;
                    fieldId: string;
                };
                cookie?: never;
            };
            requestBody: {
                content: {
                    "application/json": components["schemas"]["UpdateContentFieldDto"];
                    "text/json": components["schemas"]["UpdateContentFieldDto"];
                    "application/*+json": components["schemas"]["UpdateContentFieldDto"];
                };
            };
            responses: {
                /** @description OK */
                200: {
                    headers: {
                        [name: string]: unknown;
                    };
                    content: {
                        "text/plain": components["schemas"]["ContentFieldDto"];
                        "application/json": components["schemas"]["ContentFieldDto"];
                        "text/json": components["schemas"]["ContentFieldDto"];
                    };
                };
            };
        };
        trace?: never;
    };
}
export type webhooks = Record<string, never>;
export interface components {
    schemas: {
        ContentFieldDto: {
            /** Format: uuid */
            id: string;
            name: string;
            label: string;
            type: components["schemas"]["FieldType"];
            /** @default false */
            isRequired: boolean;
        };
        ContentTypeDto: {
            /** Format: uuid */
            id: string;
            name: string;
            kind: components["schemas"]["ContentTypeKind"];
            fields: components["schemas"]["ContentFieldDto"][];
        };
        /** @enum {unknown} */
        ContentTypeKind: "singleton" | "collection";
        CreateContentFieldDto: {
            name: string;
            label: string;
            type: components["schemas"]["FieldType"];
            /** @default false */
            isRequired: boolean;
        };
        CreateContentTypeCommand: {
            name: string;
            kind: components["schemas"]["ContentTypeKind"];
            fields: components["schemas"]["CreateContentFieldDto"][];
        };
        /** @enum {unknown} */
        FieldType: "shortText" | "longText" | "integer" | "decimal" | "boolean";
        /** @enum {unknown|null} */
        NullableOfContentTypeKind: "singleton" | "collection" | null;
        /** @enum {unknown|null} */
        NullableOfFieldType: "shortText" | "longText" | "integer" | "decimal" | "boolean" | null;
        UpdateContentFieldDto: {
            name: string | null;
            label: string | null;
            type: components["schemas"]["NullableOfFieldType"];
            isRequired: boolean | null;
        };
        UpdateContentTypeDto: {
            name: string | null;
            kind: components["schemas"]["NullableOfContentTypeKind"];
        };
    };
    responses: never;
    parameters: never;
    requestBodies: never;
    headers: never;
    pathItems: never;
}
export type $defs = Record<string, never>;
export type operations = Record<string, never>;
