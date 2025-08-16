import openApi, { astToString } from 'openapi-typescript'
import fs from 'node:fs'

const ast = await openApi('http://localhost:5139/openapi/v1.json')

fs.writeFileSync('libs/shared/util-api-types/src/lib/open-api.generated.ts', astToString(ast))
