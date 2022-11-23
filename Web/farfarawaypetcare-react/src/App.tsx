import './App.css'

import {
  ProSidebarProvider,
  Sidebar,
  Menu,
  MenuItem,
  SubMenu,
} from 'react-pro-sidebar'
import { Link, Routes, Route, useNavigate, Navigate } from 'react-router-dom'

import { InicioRoute } from './routes/Inicio/InicioRoute'
import { SobreRoute } from './routes/Sobre/SobreRoute'
import { ConfiguracaoDispositivoRoute } from './routes/Configurações/ConfiguracaoDispositivoRoute'

import GlobalContext from './contexts/global'
import { useContext, useEffect, useMemo, useState } from 'react'
import { LoginRoute } from './routes/Auth/Login'
import { RegisterRoute } from './routes/Auth/Register'
import { ConfiguracaoDispositivoDetalhesRoute } from './routes/Configurações/ConfiguracaoDispositivoDetalhesRoute'
import { RelatorioColetaRoute } from './routes/Relatório/RelatorioColetaRoute'

function App() {
  const navigate = useNavigate()
  const [user, setUser] = useState({})

  function changeUser(value: any) {
    setUser(value)
    navigate('/')
  }
  const value = useMemo(() => ({ user, changeUser }), [user])

  useEffect(() => {
    if (user) localStorage.setItem('user', JSON.stringify(user))
    else localStorage.removeItem('user')
  }, [user])

  useEffect(() => {
    let user = localStorage.getItem('user')
    // if (typeof user == 'string' && user != '{}') setUser(JSON.parse(user))
    // else setUser(null!)
  }, [])
  return (
    <GlobalContext.Provider value={value}>
      <Routes>
        <Route path="/" element={user ? <ContentLayout child={<InicioRoute />} /> : <InicioRoute />} />
        <Route path="/login" element={<LoginRoute />} />
        <Route path="/register" element={<RegisterRoute />} />
        {user ? (
          <>
            <Route
              path="/relatorio/coleta"
              element={
                <ContentLayout child={<RelatorioColetaRoute />} />
              }
            />
            <Route
              path="/configuracao/dispositivos"
              element={
                <ContentLayout child={<ConfiguracaoDispositivoRoute />} />
              }
            />
            <Route
              path="/configuracao/dispositivos/:id"
              element={
                <ContentLayout child={<ConfiguracaoDispositivoDetalhesRoute />} />
              }
            />
          </>
        ) : (
          ''
        )}
        <Route path="/sobre" element={user ? <ContentLayout child={<SobreRoute />} /> : <SobreRoute />} />
        <Route path="*" element={<Navigate replace to="/" />} />
      </Routes>
    </GlobalContext.Provider>
  )
}

export default App

function ContentLayout({ child }: { child: any }) {
  return (
    <ProSidebarProvider>
      <div className="layout-container">
        <Sidebar width="300px">
          <Menu>
            <MenuItem routerLink={<Link to="/" />}>Inicio</MenuItem>
            <SubMenu label="Relatório">
              <MenuItem routerLink={<Link to='/relatorio/coleta' />}>Relatório de Coleta</MenuItem>
            </SubMenu>
            <SubMenu label="Configurações">
              <MenuItem routerLink={<Link to="/configuracao/dispositivos" />}>
                Configurações dos Dipositivos
              </MenuItem>
            </SubMenu>
            <MenuItem routerLink={<Link to="/sobre" />}>Sobre</MenuItem>
          </Menu>
        </Sidebar>
        <div className="content-container">{child}</div>
      </div>
    </ProSidebarProvider>
  )
}
