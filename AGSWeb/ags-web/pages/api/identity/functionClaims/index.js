import FunctionClaimsHelper from '../../../../helpers/Identity/api/functionClaimsHelper'

// req = HTTP incoming message, res = HTTP server response
export default async function FunctionClaimsHandler(req, res) {
    const functionClaimsHelper = new FunctionClaimsHelper(req,res);
    const { method } = req;
    if (method == "POST"){
        const result = await functionClaimsHelper.CreateFunctionClaim(req.body.functionClaim);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

    if (method == "PUT"){
        const result = await functionClaimsHelper.UpdateFunctionClaim(req.body.functionClaim);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}