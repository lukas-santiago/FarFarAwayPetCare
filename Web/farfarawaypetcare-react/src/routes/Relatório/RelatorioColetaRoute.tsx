import { useContext, useState, useEffect } from 'react'
import { Container } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import GlobalContext from '../../contexts/global'
import { getDevices, getRelatorioColeta } from '../../services/api'
import { Device } from '../../types/types'

import "./RelatorioColetaRoute.css"

export function RelatorioColetaRoute() {
  const navigate = useNavigate()
  const ctx = useContext(GlobalContext)
  const [devices, setDevices] = useState<Device[]>([])
  const [device, setDevice] = useState<Device | null>(null)
  const [relatorioData, setRelatorioData] = useState<any>()
  const [refreshClock, setRefreshClock] = useState(60)
  const segundos = 60

  const refreshRelatorio = () => getRelatorioColeta(ctx.user.token, device?.id || devices[0].id)
    .then((res) => {
      setRelatorioData(res.data)
      setRefreshClock(segundos)
    })

  useEffect(() => {
    if (JSON.stringify(ctx.user) === JSON.stringify({})) navigate('/login')

    getDevices(ctx.user.token).then((res) => setDevices(res.data))
  }, [])

  useEffect(() => {
    if (devices.length > 0 && !relatorioData) {
      setDevice(devices[0])
      setInterval(() => {
        refreshRelatorio()
      }, segundos * 1000)
    }

  }, [devices])

  useEffect(() => {
    if (devices.length > 0)
      refreshRelatorio()
  }, [device])

  useEffect(() => {
    refreshClock > 0 && setTimeout(() => setRefreshClock(refreshClock - 1), 1000);
  }, [refreshClock]);

  function defineValue(row: any, iRow: number) {
    if (row.nome == 'Imagem')
      return (<img src={"data:image/png;base64, " + row.valueString} alt="random img" />)
    else if (row.nome == 'Alimentação')
      return "Ultima alimentação às " + row.valueString
    else if (row.nome == 'Iluminação' || row.nome == 'Tomada')
      return row.value == 0 ? "Desligado" : "Ligado"
    return row.value + row.valueString
  }

  return (
    <Container>
      <h1 className="mb-3">Relatório de Coleta</h1>
      <hr />
      <section style={{ position: 'relative' }}>
        <select onChange={(e) => setDevice(devices.find(d => d.id == Number(e.currentTarget.value))!)}>
          {devices.map((d, iD) => <option key={iD} value={d.id}>{d.nome}</option>)}
        </select>
        <div>
          Atualiza em {refreshClock}
        </div>
        <div className='relatorio-values mt-3'>
          {
            // JSON.stringify(relatorioData)
            relatorioData?.map((row: any, iRow: number) =>
              <div key={iRow}>
                <h4>{row.nome}</h4>
                <div>{defineValue(row, iRow)}</div>
              </div>)
          }
        </div>
      </section>
    </Container>
  )
}
