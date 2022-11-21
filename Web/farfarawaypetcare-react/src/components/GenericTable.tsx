import BTable from 'react-bootstrap/Table'

export function GenericTable<T extends object>({
  columns,
  data,
  actions,
  handleRow,
}: {
  columns: any[]
  data: object[]
  actions?: JSX.Element[] | undefined
  handleRow?: any
}): JSX.Element {
  return (
    <>
      <BTable className="rounded-5" striped size="sm" hover>
        <thead>
          <tr>
            {columns.map((h: any, i: number) => (
              <th key={i}>{h}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((row, iRow) => (
            <tr
              key={iRow}
              onClick={(event) => {
                typeof handleRow == 'function' ? handleRow(event, iRow) : null
              }}
            >
              {Object.keys(row).map((cellKey, iCell) => (
                <td key={iCell}>{row[cellKey as keyof typeof row]}</td>
              ))}
              <td>{actions}</td>
            </tr>
          ))}
        </tbody>
      </BTable>
    </>
  )
}
