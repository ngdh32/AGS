export const redirect_uri = process.env.ags_identity_redirect_uri; 
export const authentication_url = process.env.ags_identity_authentication_url;
export const client_secret = process.env.ags_identity_client_secret;
export const scope = 'openid profile email FunctionClaimResource ags.identity'
export const pkce_cookie_name = 'pkce'
export const auth_code_params_cookie_name = 'auth_code';
export const client_id = "AGS";