import GroupsHelper from '../../../../helpers/Identity/api/groupsHelper.js'

// req = HTTP incoming message, res = HTTP server response
export default async function GroupsHandler(req, res) {
    const groupsHelper = new GroupsHelper(req,res);
    const { method, query } = req;
    
    const { groupId } = query

    
    if (method == "DELETE"){
        const result = await groupsHelper.DeleteGroup(groupId);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}