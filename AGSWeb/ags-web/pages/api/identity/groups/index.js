import GroupsHelper from '../../../../helpers/Identity/api/groupsHelper'

// req = HTTP incoming message, res = HTTP server response
export default async function GroupsHandler(req, res) {
    const groupsHelper = new GroupsHelper(req,res);
    const { method } = req;
    if (method == "POST"){
        const result = await groupsHelper.CreateGroup(req.body.group);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

    if (method == "PUT"){
        const result = await groupsHelper.UpdateGroup(req.body.group);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}