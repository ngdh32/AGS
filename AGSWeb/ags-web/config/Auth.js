export const redirect_uri = process.env.ags_web_host + "/auth/signin-oidc"; 
export const redirect_post_logout_url = process.env.ags_web_host + "/auth/signout-callback-oidc";
export const authentication_url = process.env.ags_identity_authentication_url;
export const client_secret = process.env.ags_identity_client_secret;
export const scope = 'openid profile email FunctionClaimResource ags.identity'
export const pkce_cookie_name = 'pkce'
export const auth_code_params_cookie_name = 'auth_code';
export const auth_code_id_token_cookie_name = 'id_token';
export const client_id = "AGS";