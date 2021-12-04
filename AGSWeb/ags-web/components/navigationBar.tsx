import {
    Collapse,
    Navbar,
    NavbarToggler,
    NavbarBrand,
    Nav,
    NavItem,
    NavLink,
    UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
    NavbarText, 
    Button,
    ButtonGroup
} from 'reactstrap'
import React, { useContext, useState } from 'react';
import menus from '../config/menu'
import { AGSContext } from '../helpers/common/agsContext'
import { locale_Options } from '../config/localization'
import { SetLocaleCookieInClient, GetLocalizedString } from '../helpers/common/localizationHelper'
import "../styles/navbar.css"
import { MenuItemType } from '../models/components/menuItemType';
import { NavigationItemProps } from '../models/components/navigationItemProps';

export default function NavigationBar() {
    const agsContext = useContext(AGSContext);
    let menuOptions : MenuItemType[] = JSON.parse(JSON.stringify(menus));
    menuOptions = GetValidMenuOptions(menuOptions, agsContext.functionClaims);
    
    const [isOpen, setIsOpen] = useState(false);
    const toggle = () => setIsOpen(!isOpen);
 
    return (
        <Navbar className="ags-navbar" expand="md">
            <NavbarBrand className=""><a href="/">AGS</a></NavbarBrand>
            <NavbarToggler onClick={toggle} />
            <Collapse isOpen={isOpen} navbar>
                <Nav className="mr-auto" navbar>
                    {
                        menuOptions.map(x => {
                            return (
                                <NavigationItem menuOption={x} level={0} />
                            )
                        })
                    }
                </Nav>
                <LocaleButtonGroups />
                <NavbarText>
                    <a href="/auth/logout" className="">Logout</a>
                </NavbarText>
            </Collapse>
        </Navbar>
    )
}

function LocaleButtonGroups(){
    const agsContext = useContext(AGSContext);
    
    function OnClickLocaleButton(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>, locale: string){
        e.preventDefault();
        SetLocaleCookieInClient(locale)
        location.reload();
    }
    
    return (
        <div className="divLocale">
            {
                locale_Options.map((x, index) => {
                    const seperator = index != locale_Options.length - 1 ? "/" : "";
                    return (
                        <React.Fragment>
                            <a className={ agsContext.locale == x.value ? "locale-active" : "locale-inactive" } onClick={(e) => { OnClickLocaleButton(e, x.value)}}>{x.label}</a>
                            <span>{seperator}</span>
                        </React.Fragment>
                        // <Button color="primary" active={agsContext.locale == x.value} onClick={() => { OnClickLocaleButton(x.value) }} className="ags-link"></Button>
                    )
                })
            }
        </div>
    )
}

function NavigationItem({ menuOption, level }: NavigationItemProps) {
    if (menuOption.childrenMenus == undefined || menuOption.childrenMenus.length == 0) {
        if (level == 0) {
            // this is the toppest level
            return (
                <NavItem>
                    <NavLink><p className="">{GetLocalizedString(menuOption.labelKey)}</p></NavLink>
                </NavItem>
            )
        } else {
            // this is the lowest level
            return (
                <DropdownItem><a className="" href={menuOption.url}>{GetLocalizedString(menuOption.labelKey)}</a></DropdownItem>
            )
        }
    }
 
    if (menuOption.childrenMenus != undefined && menuOption.childrenMenus.length > 0) {
        // this is middle level and continue literation
        return (
            <UncontrolledDropdown inNavbar direction={level == 0 ? "down" : "right"}>
                <DropdownToggle nav caret className="">
                    {GetLocalizedString(menuOption.labelKey)} 
                </DropdownToggle>
                <DropdownMenu className="">
                    {
                        menuOption.childrenMenus.map(x => {
                            return (
                                <NavigationItem level={level + 1} menuOption={x} />
                            )
                        })
                    }
                </DropdownMenu>
            </UncontrolledDropdown>
        )
    }
}
 
function GetValidMenuOptions(menuOptions: MenuItemType[], functionClaims: string[]){
    const validMenuOptions: MenuItemType[] = [];
    menuOptions.forEach(x => {
        if (x.functionClaim == "" || functionClaims.some(y => y == x.functionClaim)){
            validMenuOptions.push(x);
        }
    });

    validMenuOptions.forEach(x => {
        x.childrenMenus = GetValidMenuOptions(x.childrenMenus, functionClaims);
    })

    return validMenuOptions;
}

