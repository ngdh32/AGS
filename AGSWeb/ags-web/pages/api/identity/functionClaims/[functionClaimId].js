import FunctionClaimsHelper from '../../../../helpers/identity/api/functionClaimsHelper.js'

// req = HTTP incoming message, res = HTTP server response
export default async function FunctionClaimsHandler(req, res) {
    const functionClaimsHelper = new FunctionClaimsHelper(req,res);
    const { method, query } = req;
    
    const { functionClaimId } = query

    
    if (method == "DELETE"){
        const result = await functionClaimsHelper.DeleteFunctionClaim(functionClaimId);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}